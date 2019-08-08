import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Input.less';

class Input extends Component {
    render() {
        return (
            <input className={this.getClassName()} disabled={this.props.disabled} type={this.props.type}
                   placeholder={this.props.placeholder} onChange={e => this.props.onChange(e.target.value)}
                   onKeyDown={e => this.props.onKeyDown && this.props.onKeyDown(e.key)}
            />
        );
    }

    getClassName() {
        return `${styles.input} ${this.props.className}`;
    }
}

Input.propTypes = {
    className: PropTypes.string,
    disabled: PropTypes.bool,
    placeholder: PropTypes.string,
    type: PropTypes.string,
    value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
    onChange: PropTypes.func,
    onKeyDown: PropTypes.func
};

Input.defaultProps = {
    className: '',
    type: 'text',
    disabled: false
};

export default Input;