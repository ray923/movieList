import React from "react";
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";

// core components
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import Card from "components/Card/Card.js";
import CardBody from "components/Card/CardBody.js";

import styles from "assets/jss/material-kit-react/views/landingPageSections/teamStyle.js";

const useStyles = makeStyles(styles);

export default function ContentSection(props) {
  const classes = useStyles();
  const imageClasses = classNames(
    classes.imgRaised,
    classes.imgRounded,
    classes.imgFluid
  );
  return (
    <div className={classes.section}>
      <h2 className={classes.title}>{props.Movie.Name}{props.Movie.SubName}  {props.Movie.SubTitle}</h2>
      <h4 className={classes.title} >Update Date {props.Movie.UpdateDate}</h4>
      <div>
        <GridContainer>
          <GridItem xs={12} sm={12} md={4}>
            <Card plain>
              <GridItem xs={12} sm={12} md={6} className={classes.itemGrid}>
                <img src={props.Movie.ImgUrl} alt="..." className={imageClasses} />
              </GridItem>
              <CardBody>
                <div className="commentContent" dangerouslySetInnerHTML={{ __html: props.Movie.Overview }} />
                <p className={classes.title}>
                  {props.Movie.Introduction}
                </p>
              </CardBody>
            </Card>
          </GridItem>
        </GridContainer>
      </div>
    </div>
  );
}
