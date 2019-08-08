import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Checkbox.less';

class Checkbox extends Component {
    componentDidMount() {
        this.el.indeterminate = this.props.indeterminate;
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevProps.indeterminate !== this.props.indeterminate) {
            this.el.indeterminate = this.props.indeterminate;
        }
    }

    render() {
        return (
            <div className={styles.wrapper}>
                <label className={styles.label}>
                    <input className={this.getClassName()} disabled={this.props.disabled} type='checkbox'
                           checked={this.props.checked} ref={el => (this.el = el)}
                           onChange={e => this.props.onChange(e.target.checked)}/>
                    <span className={styles.span}>{this.props.label}</span>
                </label>
            </div>
        );
    }

    getClassName() {
        return `${styles.checkbox} ${this.props.className}`;
    }
}

Checkbox.propTypes = {
    className: PropTypes.string,
    disabled: PropTypes.bool,
    label: PropTypes.string,
    checked: PropTypes.bool,
    onChange: PropTypes.func,
    indeterminate: PropTypes.bool
};

Checkbox.defaultProps = {
    className: '',
    label: '',
    disabled: false,
    indeterminate: false
};

export default Checkbox;
