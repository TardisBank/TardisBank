import * as React from 'react';
import { MenuItemType } from '../navigation/Navigation';

export const Content : React.SFC<{selectedMenuItem: MenuItemType}> = (props: {selectedMenuItem: MenuItemType}) => { 
    switch(props.selectedMenuItem) {
        case(MenuItemType.Children): 
            return <div>Children</div>;
        case(MenuItemType.Transactions):
            return <div>Transactions</div>;
        case(MenuItemType.Schedules):
            return <div>Schedules</div>
    }
    // TODO: Show the  name of the Enum
    return <div>Error: Unknown menu item: {props.selectedMenuItem}</div>
}