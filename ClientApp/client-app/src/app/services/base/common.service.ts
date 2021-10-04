import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(
  ) { }
  checkLogin(){
    const data:any = localStorage.getItem("userLogin");
    if(!data){
      return false;
    }
    if(new Date(data.expire) > new Date()){
      localStorage.removeItem("userLogin");
      return false;
    }
    return true;
  }
  setLogin(user:any){
    if(localStorage.getItem("userLogin")){
      localStorage.removeItem("userLogin");
    }
    localStorage.setItem("userLogin", JSON.stringify(user));
  }
}
