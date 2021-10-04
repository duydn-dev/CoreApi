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
  getRoles(userId: any){
    return this._baseService.get("api/get-user-role",userId);
  }
}
