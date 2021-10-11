import { environment } from '../../environments/environment';
export class ApiConfig {
    static apiUrl:string = environment.apiUrl;
    static userType:any[] = [
        {status: 0, name: "Không hiển thị"},
        {status: 1, name: "Hiển thị"}
    ]
}