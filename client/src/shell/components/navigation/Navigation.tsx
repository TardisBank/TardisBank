import * as React from 'react';
import classNames from 'classnames';
import { Drawer, List, ListItem, ListItemText, withStyles, WithStyles, ListItemIcon, IconButton } from '@material-ui/core';
import { CreditCard, People, CalendarToday, ChevronLeft } from '@material-ui/icons';
import { styles } from './Navigation.styles';

type NavigationStateProps = {
    isOpen: boolean,
    selectedMenuItem: MenuItemType
}

type NavigationDispatchProps = {
    onToobarIconClick: () => void,
    onMenuClick: (menuItem: MenuItemType) => void
}

export enum MenuItemType {
    Children,
    Transactions,
    Schedules
}

type NavigationProps = NavigationStateProps & NavigationDispatchProps & WithStyles<typeof styles>

class nav extends React.Component<NavigationProps, {}> {

    constructor(props: NavigationProps) {
        super(props);
        this.onMenuItemClick = this.onMenuItemClick.bind(this);
    }

    onMenuItemClick(menuItem: MenuItemType) {
        this.props.onMenuClick(menuItem);
    }

    render() {

        const { classes, onToobarIconClick, selectedMenuItem } = this.props;

        return (
            <Drawer
                variant="permanent"
                open={true}
                classes={{
                    paper: classNames(classes.drawerPaper, !this.props.isOpen && classes.drawerPaperClose)
                }}>
                <div className={classes.toolbarIcon}>
                    <IconButton onClick={onToobarIconClick}>
                        <ChevronLeft />
                    </IconButton>
                </div>
                <List>
                    <ListItem 
                        button={true}
                        selected={selectedMenuItem === MenuItemType.Children}
                        onClick={this.onMenuItemClick.bind(this,MenuItemType.Children)}
                        >
                        <ListItemIcon>
                            <People />
                        </ListItemIcon>
                        <ListItemText primary="Children" />
                    </ListItem>
                    <ListItem 
                        button={true}
                        selected={selectedMenuItem === MenuItemType.Transactions}
                        onClick={this.onMenuItemClick.bind(this,MenuItemType.Transactions)}
                        >
                        <ListItemIcon>
                            <CreditCard />
                        </ListItemIcon>
                        <ListItemText primary="Transactions" />
                    </ListItem>
                    <ListItem 
                        button={true}
                        selected={selectedMenuItem === MenuItemType.Schedules}
                        onClick={this.onMenuItemClick.bind(this,MenuItemType.Schedules)}
                        >
                        <ListItemIcon>
                            <CalendarToday />
                        </ListItemIcon>
                        <ListItemText primary="Schedules" />
                    </ListItem>
                </List>
            </Drawer>
        );
    }
}

export const Navigation = withStyles(styles)(nav);