import React from 'react';
import { getCandidates } from "../core/service";
import {
  keepPreviousData,
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

function CandidatesPage() {

  const columns = React.useMemo<ColumnDef<CandidateModel>[]>(
    () => [
      {
        header: 'Main',
        footer: props => props.column.id,
        columns: [
          {
            accessorFn: row => row.email,
            id: 'email',
            cell: info => info.getValue(),
            header: () => <span>Email</span>,
            footer: props => props.column.id,
          },
          {
            accessorKey: 'firstName',
            cell: info => info.getValue(),
            header: () => <span>First Name</span>,
            footer: props => props.column.id,
          },
          {
            accessorKey: 'lastName',
            cell: info => info.getValue(),
            header: () => <span>Last Name</span>,
            footer: props => props.column.id,
          },
        ],
      },
      {
        header: 'Info',
        footer: props => props.column.id,
        columns: [
          {
            accessorKey: 'phoneNumber',
            cell: info => info.getValue(),
            header: () => <span>Phone Number</span>,
            footer: props => props.column.id,
          },
          {
            accessorKey: 'linkedInUrl',
            cell: info => info.getValue(),
            header: () => <span>LinkedIn Url</span>,
            footer: props => props.column.id,
          },
          {
            accessorKey: 'gitHubUrl',
            cell: info => info.getValue(),
            header: () => <span>GitHub Url</span>,
            footer: props => props.column.id,
          },
        ],
      },
      {
        header: 'Time Interval',
        footer: props => props.column.id,
        columns: [
          {
            accessorKey: 'startTime',
            cell: info => info.getValue(),
            header: () => <span>Start Time</span>,
            footer: props => props.column.id,
          },
          {
            accessorKey: 'endTime',
            cell: info => info.getValue(),
            header: () => <span>End Time</span>,
            footer: props => props.column.id,
          },
        ],
      }
    ],
    []
  )

  const [pagination, setPagination] = React.useState<PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  })

  const dataQuery = useQuery({
    queryKey: ['data', pagination],
    queryFn: () => getCandidates(pagination),
    placeholderData: keepPreviousData, // don't have 0 rows flash while changing pages/loading next page
  })

  const defaultData = React.useMemo(() => [], [])


  const table = useReactTable({
    data: dataQuery.data?.rows ?? defaultData,
    columns,
    //pageCount: dataQuery.data?.pageCount ?? -1, //you can now pass in `rowCount` instead of pageCount and `pageCount` will be calculated internally (new in v8.13.0)
    rowCount: dataQuery.data?.rowCount, // new in v8.13.0 - alternatively, just pass in `pageCount` directly
    state: {
      pagination,
    },
    onPaginationChange: setPagination,
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true, //we're doing manual "server-side" pagination
    //getPaginationRowModel: getPaginationRowModel(), // If only doing manual pagination, you don't need this
    debugTable: true,
  })


  return (
    <div className="p-2">
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
          <li className="page-item"><a className={`page-link ${table.getCanPreviousPage() ? "" : "disabled"}`} href="#" onClick={() => table.firstPage()}>&laquo;</a></li>
          <li className="page-item"><a className={`page-link ${table.getCanPreviousPage() ? "" : "disabled"}`} href="#" onClick={() => table.previousPage()}>&lsaquo; </a></li>
          <li className="page-item"><a className={`page-link ${table.getCanNextPage() ? "" : "disabled"}`} href="#" onClick={() => table.nextPage()}>&rsaquo;</a></li>
          <li className="page-item"><a className={`page-link ${table.getCanNextPage() ? "" : "disabled"}`} href="#" onClick={() => table.lastPage()}>&raquo;</a></li>
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

export default CandidatesPage