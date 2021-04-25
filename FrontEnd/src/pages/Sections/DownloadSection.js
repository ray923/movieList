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
  function RenderOnlinePlaySection(){
    if(props.Download.OnlinePlay)
    {
      return (
        <GridItem xs={12} sm={12} md={12}>
          <h2 className={classes.title}>OnlinePaly</h2>
          <Paper 
            elevation={3} 
            children=
              {Object.keys(props.Download.OnlinePlay).map((item) => {
                return (
                  <Button color="primary"  href={props.Download.OnlinePlay[item]}>{item}</Button>
                )
              })}
          />
        </GridItem>
      )
    }
  }

  function RenderResources() {
    if(props.Download.Resource)
    {
      return (
        <GridItem xs={12} sm={12} md={12}>
          {Object.keys(props.Download.Resource).map((item) => {
            return (
              <>
                <h2 className={classes.title}>{item}</h2>
                <Paper 
                  elevation={3}
                  children=
                  {
                    Object.keys(props.Download.Resource[item]).map((e) => {
                      if(e.toString() !== "Others"){
                        return (
                          <Button color="primary" href={props.Download.Resource[item][e]}>{e}</Button>
                        )
                      }
                      else
                      {
                        return (
                          <h4>{props.Download.Resource[item][e]}</h4>
                        )
                      }
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
      <div>
        <GridContainer>
          {RenderOnlinePlaySection()}
          {RenderResources()}
        </GridContainer>
      </div>
    </div>
  );
}
