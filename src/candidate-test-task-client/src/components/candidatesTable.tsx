import React from 'react';
import { getCandidates, deleteOne } from "../core/service";
import queryClient from '../queryClient';
import {
  keepPreviousData,
  useMutation,
  useQuery,
} from '@tanstack/react-query';
import {
  PaginationState,
  useReactTable,
  getCoreRowModel,
  ColumnDef,
  flexRender,
} from '@tanstack/react-table';
import { CandidateModel } from '../core/models/candidate.model';
import { Table as BTable } from 'react-bootstrap';
import { Link } from 'react-router-dom';

function CandidatesTable() {
  const deleteMutation = useMutation(
    {
    mutationFn: (email: string) => deleteOne(email),
      onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ['candidates']});
      },
    }
  );

  const columns = React.useMemo<ColumnDef<CandidateModel>[]>(
    () => [
      {
        accessorFn: row => row.email,
        id: 'email',
        cell: info => <a href={'mailto:' + info.getValue()}>{String(info.getValue())}</a>,
        header: () => <span>Email</span>,
      },
      {
        accessorKey: 'firstName',
        cell: info => info.getValue(),
        header: () => <span>First Name</span>,
      },
      {
        accessorKey: 'lastName',
        cell: info => info.getValue(),
        header: () => <span>Last Name</span>,
      },
      {
        accessorKey: 'phoneNumber',
        cell: info => info.getValue(),
        header: () => <span>Phone Number</span>,
      },
      {
        accessorKey: 'linkedInUrl',
        cell: info => <a href={info.getValue() as string} target='_blank'>{info.getValue() as string}</a>,
        header: () => <span>LinkedIn Url</span>,
      },
      {
        accessorKey: 'gitHubUrl',
        cell: info => <a href={info.getValue() as string} target='_blank'>{info.getValue() as string}</a>,
        header: () => <span>GitHub Url</span>,
      },
      {
        accessorKey: 'timeInterval.startTime',
        cell: info => (info.getValue() as string).split('.')[0],        
        header: () => <span>Start Time</span>,
      },
      {
        accessorKey: 'timeInterval.endTime',
        cell: info => (info.getValue() as string).split('.')[0],
        header: () => <span>End Time</span>,
      },
      {
        accessorFn: (row) => row,
        id: 'actions',
        cell: ({ row }: { row: { original: CandidateModel } }) => (
          <div>
            <Link to={`/edit/${row.original.email}`}>Edit</Link>
            <button onClick={() => deleteMutation.mutate(row.original.email)}>Delete</button>
          </div>
        ),
        header: () => '',
      },      
    ],
    [deleteMutation]
  )

  const [pagination, setPagination] = React.useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const dataQuery = useQuery({
    queryKey: ['candidates', pagination],
    queryFn: () => getCandidates(pagination),
    placeholderData: keepPreviousData, 
  })

  const defaultData = React.useMemo(() => [], [])


  const table = useReactTable({
    data: dataQuery.data?.rows ?? defaultData,
    columns,
    rowCount: dataQuery.data?.rowCount,
    state: {
      pagination,
    },
    onPaginationChange: setPagination,
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true, 
    debugTable: true,
  })


  return (
    <div className="p-2">
      <Link to={`/add`}>Create new</Link>
      <BTable striped bordered hover responsive size="sm">
        <thead>
          {table.getHeaderGroups().map(headerGroup => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map(header => {
                return (
                  <th key={header.id} colSpan={header.colSpan}>
                    {header.isPlaceholder ? null : (
                      <div>
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                      </div>
                    )}
                  </th>
                )
              })}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.map(row => {
            return (
              <tr key={row.id}>
                {row.getVisibleCells().map(cell => {
                  return (
                    <td key={cell.id}>
                      {flexRender(
                        cell.column.columnDef.cell,
                        cell.getContext()
                      )}
                    </td>
                  )
                })}
              </tr>
            )
          })}
        </tbody>
      </BTable>
      <div className="h-2" />
      <div className="flex items-center gap-2">
        <ul className="pagination justify-content-center">
          <li className="page-item"><a className={`page-link ${table.getCanPreviousPage() ? "" : "disabled"}`} href="#" onClick={() => table.firstPage()}>«</a></li>
          <li className="page-item"><a className={`page-link ${table.getCanPreviousPage() ? "" : "disabled"}`} href="#" onClick={() => table.previousPage()}>‹ </a></li>
          <li className="page-item"><a className={`page-link ${table.getCanNextPage() ? "" : "disabled"}`} href="#" onClick={() => table.nextPage()}>›</a></li>
          <li className="page-item"><a className={`page-link ${table.getCanNextPage() ? "" : "disabled"}`} href="#" onClick={() => table.lastPage()}>»</a></li>
          <li className="page-item">
            <span className="page-link">
              <span>Page </span>
              <strong>
                {table.getState().pagination.pageIndex + 1} 
              </strong>
              <span> of </span>
              <strong>
                {table.getPageCount().toLocaleString()}
              </strong>
            </span>

          </li>
          <li className="page-item">
            <span className='page-link'>
              <span>Go to page: </span>
              <input
                type="number" style={{ width: '50px', height: '20px' }}
                defaultValue={table.getState().pagination.pageIndex + 1}
                onChange={e => {
                  const page = e.target.value ? Number(e.target.value) - 1 : 0
                  table.setPageIndex(page)
                }}
                className="border rounded h-full w-16 text-center"
              />            
            </span>            
          </li>
          <li className="page-item">
            <span className='page-link'>
            <select
              value={table.getState().pagination.pageSize}
              onChange={e => {
                table.setPageSize(Number(e.target.value))
              }}
              className="border rounded h-full w-16 text-center"
            >
              {[10, 20, 30, 40, 50].map(pageSize => (
                <option key={pageSize} value={pageSize}>
                  Show {pageSize}
                </option>
              ))}
            </select>
            </span>
          </li>
        </ul>

        {dataQuery.isFetching ? 'Loading...' : null}
      </div>
    </div>

  )
}
export default CandidatesTable