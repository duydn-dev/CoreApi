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

  login(username:string, password:string){
    return this._baseService.post('api/user/login',{ username, password })
  }
}
