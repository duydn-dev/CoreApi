import { ActionReducer, ActionReducerMap, createFeatureSelector, MetaReducer } from '@ngrx/store';
import * as fromUser from './reducers/user.reducer';

export interface State {
    user: fromUser.State;
}
export const reducers: ActionReducerMap<State> = {
    user: fromUser.reducer,
};

// console.log all actions
export function debug(reducer: ActionReducer<any>): ActionReducer<any> {
    return function(state, action) {
    //   console.log('state', state);
    //   console.log('action', action);
   
      return reducer(state, action);
    };
  }
   
export const metaReducers: MetaReducer<any>[] = [debug];