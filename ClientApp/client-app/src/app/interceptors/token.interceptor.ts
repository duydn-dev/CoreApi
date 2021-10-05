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
            user = JSON.parse(user);
            if(new Date(user.expire) < new Date()){
                localStorage.removeItem("user");
                //this._router.navigate(["login"]);
            }
            else{
                const modifiedReq = req.clone({ 
                    headers: req.headers.set('Authorization', `Bearer ${user.token}`),
                });
                return next.handle(modifiedReq);
            }
        }
        else{
            return next.handle(req);
        }
        
    }
}