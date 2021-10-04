import { Injectable } from '@angular/core';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private _baseService: BaseService
    ) {
  }

  login(user:any){
    return this._baseService.post('api/user/login',{...user});
  }
  getCurrentUser(){
    return localStorage.getItem('userLogin');
  }
  getRoles(userId: any){
    return this._baseService.get("api/role/get-user-role",userId);
  }
}
