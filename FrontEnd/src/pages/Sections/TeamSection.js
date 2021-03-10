import React from "react";
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";

// @material-ui/icons

// core components
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import Button from "components/CustomButtons/Button.js";
import Card from "components/Card/Card.js";
import CardBody from "components/Card/CardBody.js";
import CardFooter from "components/Card/CardFooter.js";

import styles from "assets/jss/material-kit-react/views/landingPageSections/teamStyle.js";

import team1 from "assets/img/faces/avatar.jpg";
import team2 from "assets/img/faces/christian.jpg";
import team3 from "assets/img/landing-bg.jpg";

const useStyles = makeStyles(styles);

export default function TeamSection(props) {
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
