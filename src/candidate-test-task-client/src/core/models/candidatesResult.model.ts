import { CandidateModel } from "./candidate.model";

export interface CandidatesResultModel {
	candidateDtos: CandidateModel[];
	totalCount: number;
}