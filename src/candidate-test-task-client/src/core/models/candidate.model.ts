export interface CandidateModel {
	email: string;
	firstName: string;
    lastName: string;
    phoneNumber: string;
	linkedInUrl:  string;
    gitHubUrl: string;
	timeInterval: {
		startTime: string;
		endTime: string;
	};
}
