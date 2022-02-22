import React from "react";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";

// core components
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import Paper from '@material-ui/core/Paper';

import styles from "assets/jss/material-kit-react/views/landingPageSections/productStyle.js";
import { HTMLDecode } from "../../utils/helper.js";
import { Button } from "@material-ui/core";

const useStyles = makeStyles(styles);

export default function Download(props) {
  const classes = useStyles();

  function RenderResources() {
    if(props.Download.resources)
    {
      return (
        <GridItem xs={12} sm={12} md={12}>
          {props.Download.resources.map((item) => {
            return (
              <>
                <h4 className={classes.title}>{item.formatName}</h4>
                <Paper 
                  elevation={3}
                  children=
                  {
                    item.resourceLinks.map((resource) => {
                      return (
                          <><Button color="primary" href={resource.url}>{resource.name}</Button>{HTMLDecode(resource.others)}</>
                        )
                    })
                  }
                />
              </>
            )
          })}
        </GridItem>
      )
    }
  }

  return (
    <div className={classes.section}>
      <h2 className={classes.title}>{props.Download.movieName}</h2>
      <div>
        <GridContainer>
          {RenderResources()}
        </GridContainer>
      </div>
    </div>
  );
}
