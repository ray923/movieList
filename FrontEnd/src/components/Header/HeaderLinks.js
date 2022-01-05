/*eslint-disable*/
import React from "react";
import DeleteIcon from "@material-ui/icons/Delete";
import IconButton from "@material-ui/core/IconButton";
// react components for routing our app without refresh
import { Link } from "react-router-dom";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import Tooltip from "@material-ui/core/Tooltip";

// @material-ui/icons
import { Apps, CloudDownload } from "@material-ui/icons";

// core components
import CustomDropdown from "components/CustomDropdown/CustomDropdown.js";
import Button from "components/CustomButtons/Button.js";

import styles from "assets/jss/material-kit-react/components/headerLinksStyle.js";

const useStyles = makeStyles(styles);

export default function HeaderLinks(props) {
  const classes = useStyles();
  return (
    <List className={classes.list}>
      <ListItem className={classes.listItem}>
        <CustomDropdown
          noLiPadding
          buttonText="所有电影分类"
          buttonProps={{
            className: classes.navLink,
            color: "transparent"
          }}
          buttonIcon={Apps}
          dropdownList={[
            <Link to="/" className={classes.dropdownLink}>
              所有电影
            </Link>,
            <Link to="/Cat/1" className={classes.dropdownLink}>
              美国电影
            </Link>,
            <Link to="/Cat/2" className={classes.dropdownLink}>
              其他电影
            </Link>
            // <a
            //   href="https://creativetimofficial.github.io/material-kit-react/#/documentation?ref=mkr-navbar"
            //   target="_blank"
            //   className={classes.dropdownLink}
            // >
            //   其他电影
            // </a>
          ]}
        />
      </ListItem>
      <ListItem className={classes.listItem}>
        <Button
          href="https://www.mac-downloader.com/"
          color="transparent"
          target="_blank"
          className={classes.navLink}
        >
          <CloudDownload className={classes.icons} /> 下载器
        </Button>
      </ListItem>
      <ListItem className={classes.listItem}>
        <Tooltip
          id="instagram-facebook"
          title="邮箱:gmail.com"
          placement={window.innerWidth > 959 ? "top" : "left"}
          classes={{ tooltip: classes.tooltip }}
        >
          <Button
            color="transparent"
            // href="https://www.facebook.com/CreativeTim?ref=creativetim"
            // target="_blank"
            className={classes.navLink}
          >
            <i className={classes.socialIcons + " far fa-envelope"} />
          </Button>
        </Tooltip>
      </ListItem>
      {/* <ListItem className={classes.listItem}>
        <Tooltip
          id="instagram-tooltip"
          title="Follow us on instagram"
          placement={window.innerWidth > 959 ? "top" : "left"}
          classes={{ tooltip: classes.tooltip }}
        >
          <Button
            color="transparent"
            href="https://www.instagram.com/CreativeTimOfficial?ref=creativetim"
            target="_blank"
            className={classes.navLink}
          >
            <i className={classes.socialIcons + " fab fa-instagram"} />
          </Button>
        </Tooltip>
      </ListItem> */}
    </List>
  );
}
