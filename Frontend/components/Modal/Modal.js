import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Modal.less';
import Link from "../Link/Link";
import Icon from "../Icon/Icon";

class Modal extends Component {
    render() {
        if (!this.props.visible) {
            return null;
        }

        const width = `${this.props.width}px`;

        return (
            <div className={styles.modalWrapper}>
                <div className={this.getClassName()} style={{width: width}}>
                    {this.props.children}
                    <Link className={styles.closeLink} onClick={() => this.props.onClickClose()}> <Icon
                        name='close'/></Link>
                </div>
            </div>
        );
    }

    getClassName() {
        return `${styles.modal} ${this.props.className}`;
    }
}

Modal.propTypes = {
    className: PropTypes.string,
    visible: PropTypes.bool.isRequired,
    width: PropTypes.number,
    onClickClose: PropTypes.func.isRequired
};

Modal.defaultProps = {
    className: '',
    visible: false,
    width: 300
};

export default Modal;