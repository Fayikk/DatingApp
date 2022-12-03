import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_models/Pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members : Member[] | undefined
  predicate = 'liked'
  pageNumber = 1;
  pageSize =2 ;
  pagination : Pagination | undefined

  constructor(private memberService : MembersService) { }

  ngOnInit(): void {
    this.loadLikes()
  }

  loadLikes(){
    this.memberService.getLikes(this.predicate , this.pageNumber , this.pageSize).subscribe({
      next : response => {
        this.members = response.result
        this.pagination = response.pagination
      }
    })
  }
  pageChanged(event : any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadLikes()
    }
  }

}
