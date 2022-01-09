import React from "react";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";

// core components
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import Paper from '@material-ui/core/Paper';

import styles from "assets/jss/material-kit-react/views/landingPageSections/productStyle.js";
import { Button } from "@material-ui/core";

const useStyles = makeStyles(styles);

export default function Download(props) {
  const classes = useStyles();

  function HTMLDecode(text) { 
    var temp = document.createElement("div"); 
    temp.innerHTML = text; 
    var output = temp.innerText || temp.textContent; 
    temp = null; 
    return output; 
  } 

  function RenderResources() {
    if(props.Download.Resources)
    {
      return (
        <GridItem xs={12} sm={12} md={12}>
          {props.Download.Resources.map((item) => {
            return (
              <>
                <h4 className={classes.title}>{item.FormatName}</h4>
                <Paper 
                  elevation={3}
                  children=
                  {
                    item.ResourceLinks.map((resource) => {
                      return (
                          <><Button color="primary" href={resource.Url}>{resource.Name}</Button>{HTMLDecode(resource.Others)}</>
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
      <h2 className={classes.title}>{props.Download.MovieName}</h2>
      <div>
        <GridContainer>
          {RenderResources()}
        </GridContainer>
      </div>
    </div>
  );
}
