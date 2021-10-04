import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from './ngrx';
import * as $ from 'jquery';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client-app';
  isLogin: boolean = false;
  constructor(
    private primengConfig: PrimeNGConfig,
    private store: Store<State>
  ) {
    this.primengConfig.ripple = true;
    this.store.subscribe(async n => {
      this.isLogin = n.user.isLogin;
    })
  }
}
