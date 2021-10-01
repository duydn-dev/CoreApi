import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { State } from 'src/app/ngrx/reducers/user.reducer';
import * as userActions from '../../../ngrx/actions/login.action';
import { UserService } from '../../../services/user/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup
  get form() { return this.loginForm.controls; }
  constructor(
    private _fb: FormBuilder,
    private store: Store<State>,
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      userName: this._fb.control(null, [Validators.required]),
      passWord: this._fb.control(null, [Validators.required]),
    });
  }

  loginClick() {
    if (this.loginForm.invalid) {
      return;
    }
    
    //this.store.dispatch(userActions.login({user: { userId: 1, userName: "duydn"}}));
  }
}
