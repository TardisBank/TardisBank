import * as React from 'react';
import classNames from 'classnames';
import { Drawer, List, ListItem, ListItemText, withStyles, WithStyles, ListItemIcon, IconButton, Avatar } from '@material-ui/core';
import { ChevronLeft } from '@material-ui/icons';
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

    onMenuItemClick (menuItem: MenuItemType) {
        this.props.onMenuClick(menuItem);
    }

    private makeColour(name: string) {
        let hash = 0;
        for (let i = 0; i < name.length; i++){
            hash = name.charCodeAt(i) + ((hash << 5) - hash);
        }

        let colour = '#';
        for (let i = 0; i < 3; i++) {
            let value = (hash >> (i * 8)) & 0xFF;
            colour += ('00' + value.toString(16)).substr(-2);
        }

        return colour;
    }

    private listItem(menuType: MenuItemType, name: string) {
        const isSelected = this.props.selectedMenuItem === menuType
        return (
            <ListItem
                button={true}
                selected={isSelected}
                onClick={this.onMenuItemClick.bind(this, menuType)}
            >
                <ListItemIcon>
                    <Avatar style={{backgroundColor: this.makeColour(name)}}>
                        {name[0]}
                    </Avatar>
                </ListItemIcon>
                <ListItemText primary={name} />
            </ListItem>
        )
    }

    render() {

        const { classes, onToobarIconClick } = this.props;

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
                    {this.listItem(
                        MenuItemType.Children,
                        "Children")}
                    {this.listItem(
                        MenuItemType.Transactions,
                        "Transactions")}
                    {this.listItem(
                        MenuItemType.Schedules,
                        "Schedules")}
                </List>
            </Drawer>
        );
    }
}

export const Navigation = withStyles(styles)(nav);