import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  // Get token from local storage
  const token = localStorage.getItem("token");

   // Add bearer token:
   const clonedRequest = req.clone({
    setHeaders: {
        Authorization:`Bearer ${token}`
    }
});

  // Continue to the next interceptor
  return next(clonedRequest);
};
