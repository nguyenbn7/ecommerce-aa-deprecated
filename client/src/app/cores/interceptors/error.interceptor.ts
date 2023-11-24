import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((response: HttpErrorResponse) => {
        console.log(response.status);
        if (response) {
          switch (response.status) {
            case 400:
              if (response.error.errors) {
                throw response.error;
              } else {
                this.toastr.error(
                  response.error.message,
                  response.status.toString()
                );
              }
              break;
            case 401:
              this.toastr.error(
                response.error.message,
                response.status.toString()
              );
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: response.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              break;
          }
        }
        return throwError(() => Error(response.message));
      })
    );
  }
}
