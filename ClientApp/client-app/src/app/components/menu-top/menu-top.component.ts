import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { State } from 'src/app/ngrx';
import * as userActions from '../../ngrx/actions/login.action';

@Component({
  selector: 'app-menu-top',
  templateUrl: './menu-top.component.html',
  styleUrls: ['./menu-top.component.css']
})
export class MenuTopComponent implements OnInit {

  currentUser:any = {};
  constructor(
    private _store : Store<State>,
    private _router: Router
  ) { }

  ngOnInit(): void {
    this._store.subscribe(n => {
      this.currentUser = n.user.user;
    })
  }
  logout(){
    localStorage.removeItem('user');
    this._router.navigate(["/login"]);
  }
}
