import { MutationCache, QueryClient } from '@tanstack/react-query';
import { ApiError } from './core/models/apierror.model';
import axios from 'axios';

export const isApiErrorResponse = (res: unknown): res is ApiError => {
    return (
      typeof res === 'object' &&
      res !== null &&
      "type" in res &&
      "title" in res &&
      "status" in res &&
      "errors" in res &&
      typeof res.errors === 'object' &&
      res.errors !== null &&
      Object.values(res.errors).every(errors => Array.isArray(errors))
    );
  }

  export const handleErrorMessage = (error: unknown) => {
    if (!axios.isAxiosError(error)) {
      return "Unknown error";
    }
  
    if (!error.response || !isApiErrorResponse(error.response.data)) {
      return error.message;
    }
  
    return error.response.data.title;
  }; 

 const queryClient = new QueryClient({
    mutationCache: new MutationCache({
      onError: (error, _variables, _context, mutation) => {
        if (mutation.options.onError) return;

        const errorMessage = handleErrorMessage(error);
        console.error(errorMessage);
        alert(errorMessage);
      },
    }),
  });

export default queryClient;
