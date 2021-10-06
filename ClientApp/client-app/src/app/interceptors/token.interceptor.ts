import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(private _router: Router) {
        
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let user:any = localStorage.getItem("user");
        if(user){
            const modifiedReq = req.clone({ 
                headers: req.headers.set('Authorization', `Bearer ${user?.token}`),
            });
            return next.handle(modifiedReq);
        }
        return next.handle(req);
    }
}