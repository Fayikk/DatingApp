import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl
  members : Member[] = [];
  paginatedResult : PaginatedResult<Member[]> = new PaginatedResult<Member[]>;

  constructor(private http:HttpClient) { }


  getMembers(page?:number , itemsPerPage?: number){
    
    let params = new HttpParams();
    if (page && itemsPerPage) {
      params = params.append('pageNummber' , page);
      params = params.append('pageSize' , itemsPerPage)
    }
    return this.http.get<Member[]>(this.baseUrl+'users' , {observe : 'response' , params}).pipe(
      map(response => {
        if (response.body) {
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      })
     )


  }


  getMember(username:string){
    const member = this.members.find(x=>x.userName == username)
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl+"users/"+username)
  }


  updateMember(member:Member){
    return this.http.put(this.baseUrl+'users',member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index],...member}
        //with for spinner code
      })
    )
  }
 

  setMainPhoto(photoId : number){
    return this.http.put(this.baseUrl+ "users/set-main-photo/" + photoId,{})
  }

  deletePhoto(photoId : number){
    return this.http.delete(this.baseUrl + "users/delete-photo/"+photoId);
  }

}
