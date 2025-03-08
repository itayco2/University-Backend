import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UserModel } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';
import {  Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,CommonModule,MatFormFieldModule, MatInputModule, MatCheckboxModule, MatButtonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {


    private user = new UserModel();
    public userForm: FormGroup;

    public constructor(
        private userService: UserService,
        private router: Router,
        private formBuilder: FormBuilder, 
        private notifyService: NotifyService
    ) { }


    ngOnInit(): void {
       this.userForm = this.formBuilder.group({
        NameControl: new FormControl(""),
        emailControl: new FormControl(""),
        passwordControl: new FormControl(""),
       });
    }

    public async send(){
        try{
            this.user.name = this.userForm.get("NameControl").value;
            this.user.email = this.userForm.get("emailControl").value;
            this.user.password = this.userForm.get("passwordControl").value;
            await this.userService.register(this.user);
            this.notifyService.success(`Welcome ${this.user.name}`);
            this.router.navigateByUrl("/home");
        }
        catch(err : any){
            this.notifyService.error(err);
        }
    }




}
