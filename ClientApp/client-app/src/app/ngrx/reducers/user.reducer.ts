import { Action, createReducer, on } from '@ngrx/store';
import * as userActions from '../actions/login.action';
import { User } from '../entities/user.entity';

export interface State {
    user: User;
    isLogin: boolean;
}

export const initialState: State = {
    user: new User(),
    isLogin: false
};

const loginReducer = createReducer(
    initialState,
    on(userActions.login, (state, {user}) =>  ({ ...state, user, isLogin: true })),
);

export function reducer(state: State | undefined, action: Action): any {
    return loginReducer(state, action);
}