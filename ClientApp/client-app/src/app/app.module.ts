import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/users/login/login.component';
import { StoreModule } from '@ngrx/store';
import {metaReducers, reducers} from '../app/ngrx/index';

// primNG module
import {ToastModule} from 'primeng/toast';
import {TableModule} from 'primeng/table';
import { MessageService } from 'primeng/api';
import {PaginatorModule} from 'primeng/paginator';


import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { AuthGuard } from './guard/auth-guard';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { MenuLeftComponent } from './components/menu-left/menu-left.component';
import { MenuTopComponent } from './components/menu-top/menu-top.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    NotfoundComponent,
    MenuLeftComponent,
    MenuTopComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    ToastModule,
    TableModule,
    PaginatorModule,
    StoreModule.forRoot(reducers, {metaReducers})
  ],
  providers: [
    MessageService,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
