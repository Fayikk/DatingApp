import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/Pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members : Member[] = [];
  pagination: Pagination | undefined
  pageNumber = 1; // ->2
  pageSize = 5;

  constructor(private memberService:MembersService,private toastr:ToastrService) { }

  ngOnInit(): void 
  {
      // this.members$ = this.memberService.getMembers();  
      this.loadMembers();
   
  }

  loadMembers(){
    this.memberService.getMembers(this.pageNumber , this.pageSize).subscribe({
      next : response => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;

        }
      }
    })
  }

  pageChanged(event : any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadMembers()
    }
  }


}
