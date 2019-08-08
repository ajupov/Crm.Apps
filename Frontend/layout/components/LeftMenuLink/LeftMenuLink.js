import React, {Component} from 'react';
import PropTypes from 'prop-types';
import Icon from '../../../components/Icon/Icon';
import Link from '../../../components/Link/Link';
import styles from './LeftMenuLink.less';

class LeftMenuLink extends Component {
    render() {
        return (
            <Link value={this.props.value} className={styles.link} activeClassName={styles.activeLink}>
                <Icon name={this.props.icon} className={styles.icon}/>
                <span className={styles.text}>{this.props.text}</span>
            </Link>
        );
    }
}

LeftMenuLink.propTypes = {
    value: PropTypes.string.isRequired,
    icon: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired
};

export default LeftMenuLink;