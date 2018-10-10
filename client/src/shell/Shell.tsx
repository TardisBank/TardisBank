import * as React from 'react';
import { WithStyles, withStyles } from '@material-ui/core';
import { Header } from './components/header/Header';
import { Navigation, MenuItemType } from './components/navigation/Navigation'
import { Content } from './components/content/Content';
import { styles } from './Shell.styles';

type ShellProps = WithStyles<typeof styles>;

type ShellState = {
    isSideBarOpen: boolean,
    selectedMenuItem: MenuItemType
};

class shell extends React.Component<ShellProps, ShellState> {

    state = { isSideBarOpen: false, selectedMenuItem: MenuItemType.Children }

    constructor(props: ShellProps) {
        super(props);

        this.handleSidebarClick = this.handleSidebarClick.bind(this);
        this.onMenuClick = this.onMenuClick.bind(this);
    }


    handleSidebarClick() {
        this.setState(state => {
            return {
                ...state,
                isSideBarOpen: !state.isSideBarOpen
            }
        });
    }

    onMenuClick(selectedMenuItem: MenuItemType) {
        this.setState(state => {
            return {
                ...state,
                selectedMenuItem
            }
        })
    }

    render() {
        const { classes } = this.props;
        return (
            <div className={classes.root}>
                <Header isOpen={this.state.isSideBarOpen} onMenuClick={this.handleSidebarClick} />
                <Navigation
                    isOpen={this.state.isSideBarOpen}
                    onToobarIconClick={this.handleSidebarClick}
                    selectedMenuItem={this.state.selectedMenuItem}
                    onMenuClick={this.onMenuClick}
                />
                <main className={classes.content}>
                    <div className={classes.appBarSpacer} />
                    <Content selectedMenuItem={this.state.selectedMenuItem}/>
                </main>
            </div>);

    }
}



export const Shell = withStyles(styles)(shell);