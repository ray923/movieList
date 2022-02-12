import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';

const useStyles = makeStyles({
  root: {
    maxWidth: 345,
  },
  media: {
    maxwidth: 284,
    minwidth: 142,
    minheight: 200,
    height: 400
  },
});



export default function MediaCard(props) {
  const classes = useStyles();

  return (
    <Card className={classes.root}>
      <CardActionArea href={`/content/` + props.Id}>
        <CardMedia
          className={classes.media}
          image={props.ListImgUrl}
          title={props.Name}
        />
        <CardContent>
          <Typography gutterBottom variant="h5" component="h2">
            {props.Name}
          </Typography>
          <Typography variant="body2" color="textSecondary" component="p">
            {props.SubTitle}
          </Typography>
        </CardContent>
      </CardActionArea>
      <CardActions>
        <Button size="small" color="primary" href={`/content/` + props.Id}>
          Learn More
        </Button>
      </CardActions>
    </Card>
  );
}
