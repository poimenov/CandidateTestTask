import axios from "axios";
import { apiConfig } from "../config";

const api = axios.create({
	baseURL: apiConfig.candidatesSeviceURI,
});

api.interceptors.request.use((req) => {
	return {
		...req,
		baseURL: apiConfig.candidatesSeviceURI,
	};
});

export { api };
