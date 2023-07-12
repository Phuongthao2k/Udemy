import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { filter, from, map, of, pipe } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestService {

}

const numbers = of(1,2,3,4,5);

const squareOddVals = pipe(
    filter((n: number) => n % 2  !== 0),
    map(n => n*n)
)


