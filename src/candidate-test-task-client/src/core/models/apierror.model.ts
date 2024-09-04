export type ApiError = {
    type: string;
    title: string;
    status: number;
    errors: {
      [key: string]: string[];
    }
  };