import { Component, OnInit } from '@angular/core';
declare var $:any;

@Component({
  selector: 'app-menu-left',
  templateUrl: './menu-left.component.html',
  styleUrls: ['./menu-left.component.css']
})
export class MenuLeftComponent implements OnInit {

  menuData:any[] = [];
  constructor() { }

  ngOnInit(): void {
    this.menuData = [
      {
        name: "Dashboard",
        link: "/",
        childs:[
          {name: "Dashboard Child 1", link: "/link1"},
          {name: "Dashboard Child 2", link: "/link2"},
          {name: "Dashboard Child 3", link: "/link3"},
        ] 
      },
      {
        name: "Phòng họp",
        link: "/phonghop",
        childs: [
          {
            name: "PhongHop 1", 
            link: "/phonghop1", 
          },
          {
            name: "PhongHop 2",
            link: "/phonghop2"
          },
        ]
      },
      {name: "Hệ thống", link: "/hethong"},
    ]
  }
  
  menuClick(e){
    if($(e.target).next().attr('class') == "mdl-navigation"){
      $(e.target).parent().parent().toggleClass('sub-navigation--show');
      $(e.target).toggleClass('mdl-navigation__link--current');
    }
  }
}
