// đây là action
import { createAction, props } from '@ngrx/store';
import { User } from '../entities/user.entity';
export const USER_LOGIN = '[Login Page] Login';

export const login = createAction(
  USER_LOGIN,
  props<{user: User}>()
);