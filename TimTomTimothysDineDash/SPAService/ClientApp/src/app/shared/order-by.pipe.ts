import { Pipe, PipeTransform } from '@angular/core';
import { orderBy } from 'lodash';

@Pipe({
  name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {

  transform(list: any, sortOn: string, isOrderAscending: boolean): any {
    return orderBy(list, sortOn, isOrderAscending ? 'asc' : 'desc') ?? [];
  }

}
