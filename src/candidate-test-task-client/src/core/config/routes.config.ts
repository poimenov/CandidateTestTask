import { config } from "./api.config";

export const candidatesAPIRoutes = {
	getPage: (page: number, pageSize: number) => config().api(`/candidates/${page}/${pageSize}`),
    getCount: () => config().api(`/candidates/count`),
    getOne: (email: string) => config().api(`/candidate/${email}`),
    deleteOne: (email: string) => config().api(`/candidate/${email}`),  
    postOne: () => config().api(`/candidate`),
};

