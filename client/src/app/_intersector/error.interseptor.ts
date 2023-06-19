import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NavigationExtras, Route, Router } from "@angular/router";
import { Observable, catchError } from "rxjs";

@Injectable()
export class ErrorInterseptor implements HttpInterceptor {

    constructor(private router: Router) {
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error) {
                    switch (error.status) {
                        case 400:
                            if (error.error.errors) {
                                // const modelStateError = [];

                                // throw modelStateError;
                            }
                            break;
                        case 401:
                            break;
                        case 404:
                            this.router.navigateByUrl("/not-found")
                            break;
                        case 500:
                            const navigationExtrasa: NavigationExtras = {state: {error: error.error}}
                            this.router.navigateByUrl("/server-error",navigationExtrasa);
                            break;
                        default: 
                            console.log(error);
                    }
                }

                throw error;
            })
        )
    }

}