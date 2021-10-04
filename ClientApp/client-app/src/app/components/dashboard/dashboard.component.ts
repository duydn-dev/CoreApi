import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    const currentUser:any = JSON.parse(this._userService.getCurrentUser());
    //this.getUserRole(currentUser.userId);
  }
  getUserRole(userId: any){
    this._userService.getRoles(userId).subscribe(n => {
      console.log(n)
    })
  }
}
