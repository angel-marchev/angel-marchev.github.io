function [rec,best,iter]=gadove(x,y,z,sz)
%% initialize
candidate=round(rand(15,3)*sz)-1; %15 бро�? гадове
%objective=round(rand(1,3)*sz,0)-1;
objective=[x y z];
eucl=ones(size(candidate,1),1);
iter=0;
limit=5;
[B, I]=mink(eucl,5);
best=candidate(I(1),:);
rec=[0 0 0];
precision=round(0.05*sz);
%% begin selection, set terminal conditions
while abs(best(1)-objective(1))>precision ||...
      abs(best(2)-objective(2))>precision ||...
      abs(best(3)-objective(3))>precision %min(eucl)~=0
%% calculate euclidean distance
for i=1:size(candidate,1)
eucl(i) = sqrt(sum((candidate(i,:) - objective) .^ 2));
end
%% histogram
histo=1000-eucl;
%% select best 5 solutions
[B, I]=mink(eucl,5);
best=candidate(I(1),:);
ind=zeros(size(candidate,1),3);
ind(I,:)=1;
kill=candidate.*ind;
candidate=sortrows(kill,'descend');
%% new generation by full pairwise combination
for i=1:3
candidate(6,i)=round(mean([candidate(1,i) candidate(2,i)]));
end
for i=1:3
candidate(7,i)=round(mean([candidate(1,i) candidate(3,i)]));
end
for i=1:3
candidate(8,i)=round(mean([candidate(1,i) candidate(4,i)]));
end
for i=1:3
candidate(9,i)=round(mean([candidate(1,i) candidate(5,i)]));
end
for i=1:3
candidate(10,i)=round(mean([candidate(2,i) candidate(3,i)]));
end
for i=1:3
candidate(11,i)=round(mean([candidate(2,i) candidate(4,i)]));
end
for i=1:3
candidate(12,i)=round(mean([candidate(2,i) candidate(5,i)]));
end
for i=1:3
candidate(13,i)=round(mean([candidate(3,i) candidate(4,i)]));
end
for i=1:3
candidate(14,i)=round(mean([candidate(3,i) candidate(5,i)]));
end
for i=1:3
candidate(15,i)=round(mean([candidate(4,i) candidate(5,i)]));
end
%% maintanance 
iter=iter+1;
if iter==limit
    break
end
%message=['Best match = ', num2str(best), ', iteration: ', num2str(iter),' , objective =',num2str(objective)];
%message
rec=[rec;best];
%% mutation, if stuck in same solution 
if rec(iter+1,:)==rec(iter,:)
   mutate=(rand(3,1).*30)-15;
   for i=1:size(candidate,1)
   candidate(i,:)=round(bsxfun(@plus,candidate(i,:),mutate'));
   end
end
%% capping, if any is <0, >255
    for i=1:45
       if candidate(i)<0
           candidate(i)=0;
       elseif candidate(i)>255
           candidate(i)=255;
       end
    end
end
