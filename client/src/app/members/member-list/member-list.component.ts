import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { Observable } from 'rxjs';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  // members$: Observable<PaginatedResult<Member[]>> | undefined;
  members: Member[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  
  ngOnInit(): void {
    this.loadMembers();
  }

  constructor(private memberService: MembersService) {}


  loadMembers() {
    this.memberService.getMembers(this.pageNumber,this.pageSize).subscribe({
      next: respose => {
        if(respose.result && respose.pagination) {
          this.members = respose.result;
          this.pagination = respose.pagination
        }
      }
    })
  }

  pageChanged(event: PageChangedEvent) {
    if(this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMembers();
    }
  }

}
