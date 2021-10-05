import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(
  ) { }
  checkLogin(){
    const data:any = localStorage.getItem("user");
    if(!data){
      return false;
    }
    if(new Date(data.expire) > new Date()){
      localStorage.removeItem("user");
      return false;
    }
    return true;
  }
  setLogin(user:any){
    if(localStorage.getItem("user")){
      localStorage.removeItem("user");
    }
    localStorage.setItem("user", JSON.stringify(user));
  }
}
