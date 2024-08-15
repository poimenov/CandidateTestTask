import { api } from "./api";
import { CandidatesResultModel } from "../models/candidatesResult.model";
//import { PaginationItemsModel } from "../models/paginationItems.model";
import { CandidateModel } from "../models/candidate.model";
import { candidatesAPIRoutes } from "../config/routes.config";

export async function getCandidates(options: {
    pageIndex: number
    pageSize: number
  }) {
    const data = await api.get<CandidatesResultModel>(candidatesAPIRoutes.getPage(options.pageIndex + 1, options.pageSize))
        .then(response => response.data);
 
    return {
        rows: data.candidateDtos,
        pageCount: Math.ceil(data.totalCount / options.pageSize),
        rowCount: data.totalCount,
      }      
}

export const getCount = (): Promise<number> =>
    api.get<number>(candidatesAPIRoutes.getCount())
    .then(response => response.data);

export const getOne = (email: string): Promise<CandidateModel> =>
    api.get<CandidateModel>(candidatesAPIRoutes.getOne(email))
    .then(response => response.data);

export const deleteOne = (email: string) =>
    api.delete(candidatesAPIRoutes.deleteOne(email));

export const postOne = (candidate: CandidateModel) =>
    api.post(candidatesAPIRoutes.postOne(), candidate);

