import React, {Component} from 'react';
import PropTypes from 'prop-types';
import Link from '../../../components/Link/Link';
import styles from './TopMenuLink.less';

class TopMenuLink extends Component {
    render() {
        return (
            <Link value={this.props.value} className={styles.link} activeClassName={styles.activeLink}>
                <span className={styles.text}>{this.props.text}</span>
            </Link>
        );
    }
}

TopMenuLink.propTypes = {
    value: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired
};

export default TopMenuLink;