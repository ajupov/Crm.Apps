import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import styles from './Link.less';

class Link extends Component {
    render() {
        if (this.props.force) {
            return (
                <a className={this.getClassName()} href={this.props.value} target={this.getTarget()}
                   disabled={this.props.disabled}>
                    {this.props.children}
                </a>
            );
        }

        if (this.props.value && (!this.props.target || this.props.target === 'self')) {
            return (
                <NavLink className={this.getClassName()} to={this.props.value} disabled={this.props.disabled}
                         activeClassName={this.getActiveClassName()}>
                    {this.props.children}
                </NavLink>
            );
        }

        return (
            <a className={this.getClassName()} href={this.props.value} disabled={this.props.disabled}
               target={this.getTarget()} onClick={e => this.onClick(e)}>
                {this.props.children}
            </a>
        );
    }

    getClassName() {
        return `${styles.a} ${this.props.className}`;
    }

    getActiveClassName() {
        return window.location.pathname === this.props.value ? this.props.activeClassName : '';
    }

    getTarget() {
        switch (this.props.target) {
            case 'blank':
                return '_blank';
            case 'parent':
                return '_parent';
            case 'top':
                return '_top';
            case 'self':
            default:
                return '_self';
        }
    }

    onClick(e) {
        e.preventDefault();
        e.stopPropagation();

        this.props.onClick();
    }
}

Link.propTypes = {
    className: PropTypes.string,
    activeClassName: PropTypes.string,
    value: PropTypes.string,
    disabled: PropTypes.bool,
    force: PropTypes.bool,
    target: PropTypes.oneOf(['blank', 'parent', 'self', 'top']),
    onClick: PropTypes.func
};

Link.defaultProps = {
    className: '',
    disabled: false,
    force: false,
    value: '',
    target: 'self'
};

export default Link;