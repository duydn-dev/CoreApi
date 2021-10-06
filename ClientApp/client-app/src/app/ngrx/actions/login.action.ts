// đây là action
import { createAction, props } from '@ngrx/store';
import { User } from '../entities/user.entity';
export const USER_LOGIN = '[Login Page] Login';
export const USER_LOGOUT = '[Logout Page] Logout';

export const login = createAction(
  USER_LOGIN,
  props<{user: User}>()
);
export const logout = createAction(
  USER_LOGOUT
);