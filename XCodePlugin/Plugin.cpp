/*
	This is a simple plugin, a bunch of functions that do simple things.
*/

#include "Plugin.pch"
#include <Accelerate/Accelerate.h>
//#include <iostream>
//using namespace std;


//#define VDSP_C
//#define TRIANG_C

static float kSpring = .1f;
static int currentSize=0;
static float** similarMat;
static Vector3 * lastPos=NULL;






void updatePhysics(Vector3 *  res, int size, float deltaTime){
//    cout << "update" << endl;
//    memccpy(lastPos, res,size,sizeof(Vector3));
    for (int i = 0 ; i < size ; i++){
        lastPos[i]=res[i];
    };

#ifdef TRIANG_C
    for (int i = 0 ; i < size; i++){
        for (int j = 0 ; j < size; j++){
            Vector3 dist = lastPos[i]-lastPos[j];
            float l0 ;
            if(j<i){
                l0 = similarMat[j][i];
            }
            else if(i>j){
                l0 = similarMat[i][j];
            }
            res[i]+= dist*1.0f/dist.mag()*(dist.mag()-l0)*kSpring*deltaTime;
            
        }
    }
#else
    for (int i = 0 ; i < size; i++){
//        res[i]+= Vector3(0,l0[i][j],0);
        Vector3 force = Vector3(0,0,0);
        for (int j = 0 ; j < size; j++){
            if(i!=j){
            Vector3 dist = lastPos[i]-lastPos[j];
            float l0 ;

            l0 = similarMat[i][j];
            
                force+= dist*1.0f/(std::sqrt(dist.mag()))*(std::sqrt(dist.mag())-l0)*kSpring*deltaTime;
            }
        
        }
        
        res[i]+=force*deltaTime;
    }
    
#endif

}





void setSpring(float k){
    kSpring = k;
}





void addElement(int id1,int id2,float sim){
    int max = MAX(id1,id2)+1;

    resize(max);
    
#ifdef TRIANG_C
    if(id2<=id1){
        similarMat[id1][id2] = sim;
    }
    else{
        similarMat[id2][id1] = sim;
    }
#else
    similarMat[id1][id2] = sim;
    similarMat[id2][id1] = sim;
#endif
    
}





void destroydata(){
    
    for(int i = 0 ; i < currentSize ; i++){
        delete[] similarMat[i];
    }
    delete[] similarMat;
//    similarMat =NULL;
}







void resize(int max){
    if(currentSize==0){
        
        if(similarMat!=NULL)destroydata();
        if(lastPos!=NULL)delete[] lastPos;
        
        lastPos = new Vector3[max];
        similarMat = new float*[max];
        lastPos = new Vector3[max];
        for(int i = 0 ; i < max;i++){
#ifdef TRIANG_C 
            similarMat[i]=new float[max-i];
            for(int j = 0 ; j < max-i;j++){
                similarMat[i][j]=0;
            }
#else
            similarMat[i]=new float[max];
            for(int j = 0 ; j < max;j++){
                similarMat[i][j]=0;
            }
#endif
        }
        
    }
    else{
        //TODO : Check
        if(max>currentSize){
        //init
        float ** tmp = new float*[max];
        for(int i = 0 ; i < max;i++){
            
#ifdef TRIANG_C
            tmp[i]=new float[max-i];
            if( i < currentSize){
                memccpy(tmp[i], similarMat[i], currentSize-i, sizeof(float));
                for(int j = currentSize-1 ; j< max-i ; j++){
                    tmp[i][j]=0;
                }
            }
            else{
                for(int j = 0 ; j< max-i ; j++){
                    tmp[i][j]=0;
                }
            }
#else
            tmp[i]=new float[max];
            if( i < currentSize){
                memccpy(tmp[i], similarMat[i], currentSize, sizeof(float));
                for(int j = currentSize-1 ; j< max ; j++){
                    tmp[i][j]=0;
                }
            }
            else{
            for(int j = 0 ; j< max ; j++){
                tmp[i][j]=0;
            }
            }
#endif
            }

            
        
        //destroy
        destroydata();
        
        //swap
        similarMat = &tmp[0];
            delete[] lastPos;
            lastPos = new Vector3[max];
            currentSize = max;
        }
        
    }
    
    
    
}



